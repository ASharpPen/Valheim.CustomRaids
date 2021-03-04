using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Valheim.CustomRaids.ConfigurationCore
{
    public static class ConfigurationLoader
    {
        public static void ScanBindings<TGroup, TSection>(ConfigFile config, bool debug)
            where TGroup : ConfigurationGroup<TSection>
            where TSection : ConfigurationSection
        {
            var lines = File.ReadAllLines(config.ConfigFilePath);

            var groupFields = typeof(TGroup).GetFields().ToDictionary(x => x.Name);
            var sectionFields = typeof(TSection).GetFields().ToDictionary(x => x.Name);

            string lastSection = null;
            bool lastSectionWasHeader = false;

            foreach (var line in lines)
            {
                if (line.StartsWith("["))
                {
                    string sectionName = new Regex(@"(?<=[[]).+(?=[]])").Match(line).Value;

                    lastSection = sectionName;

                    lastSectionWasHeader = lastSection.Split('.').Length == 1;
                }
                else if (line.Length > 0 && line.Contains("="))
                {
                    var keyValue = line.Split('=');

                    if (keyValue.Length == 2)
                    {
                        string key = keyValue[0].Trim();

                        if (debug) CustomLog.LogTrace($"Attempting to bind configuration {lastSection}:{key}");

                        var fields = lastSectionWasHeader
                            ? groupFields
                            : sectionFields;

                        if(fields.ContainsKey(key))
                        {
                            var field = fields[key];

                            var entry = (IConfigurationEntry)Activator.CreateInstance(field.FieldType);
                            entry.Bind(config, lastSection, key);

                            if (debug) CustomLog.LogTrace($"Successfully bound '{field.Name}'.");
                        }
                    }
                }
            }
        }

        public static Dictionary<string, TGroup> LoadArrayConfigurations<TGroup, TSection>(ConfigFile configFile, bool debug)
            where TGroup : ConfigurationGroup<TSection>
            where TSection : ConfigurationSection
        {
            if (debug)
            {
                CustomLog.LogTrace("Keys available for binding:");
                foreach (var key in configFile.Keys)
                {
                    CustomLog.LogTrace(key.Key);
                }
            }

            var sectionKeys = configFile.Keys.GroupBy(x => x.Section).ToList();

            var configurations = new Dictionary<string, TGroup>(sectionKeys.Count());

            foreach (var sectionKey in sectionKeys)
            {
                if (debug) CustomLog.LogTrace($"Binding group: {sectionKey.Key}");

                var components = sectionKey.Key.Split('.');

                string groupName = components[0];
                string elementKey = components.Length > 1
                    ? components[1]
                    : null;

                TGroup group = GetOrInitializeGroup<TGroup, TSection>(configurations, groupName);
                Dictionary<string, TSection> groupSections = group.Sections;
                Dictionary<string, IConfigurationEntry> groupEntries = group.Entries;

                if (string.IsNullOrEmpty(elementKey))
                {
                    //Assume that this is a group definition. Load in group entries.
                    BindObjectEntries(configFile, group, groupName, debug);
                }
                else
                {
                    TSection section = GetOrInitializeSection(groupSections, elementKey);
                    Dictionary<string, IConfigurationEntry> entries = section.Entries;

                    //Start loading in the configuration key:values of the section
                    BindObjectEntries(configFile, section, sectionKey.Key, debug);
                }
            }

            return configurations;
        }

        private static void BindObjectEntries(ConfigFile configFile, IHaveEntries configuration, string sectionHeader, bool debug)
        {
            var fields = configuration
                    .GetType()
                    .GetFields()
                    .ToList();

            foreach (var field in fields)
            {
                if (debug) CustomLog.LogTrace($"Creating and binding entry for '{sectionHeader}:{field.Name}'");

                var entry = (IConfigurationEntry)field.GetValue(configuration);
                entry.Bind(configFile, sectionHeader, field.Name);

                configuration.Entries[field.Name] = entry;
                field.SetValue(configuration, entry);

                if (debug) Debug.Log($"[{sectionHeader}]: Loaded [{field.Name}:{entry}]");
            }
        }

        private static TGroup GetOrInitializeGroup<TGroup, TSection>(Dictionary<string, TGroup> groups, string groupName)
            where TGroup : ConfigurationGroup<TSection>
            where TSection : ConfigurationSection
        {
            TGroup group;

            if (!groups.ContainsKey(groupName))
            {
                groups.Add(groupName, group = Activator.CreateInstance<TGroup>());
                group.GroupName = groupName;
                group.Sections = new Dictionary<string, TSection>();
                group.Entries = new Dictionary<string, IConfigurationEntry>();
            }
            else
            {
                group = groups[groupName];
            }

            return group;
        }

        private static TSection GetOrInitializeSection<TSection>(Dictionary<string, TSection> sections, string sectionKey) 
            where TSection : ConfigurationSection
        {
            TSection section;

            if (!sections.ContainsKey(sectionKey))
            {
                sections.Add(sectionKey, section = Activator.CreateInstance<TSection>());
                section.SectionName = sectionKey;
                section.Entries = new Dictionary<string, IConfigurationEntry>();
            }
            else
            {
                section = sections[sectionKey];
            }

            return section;
        }
    }
}
