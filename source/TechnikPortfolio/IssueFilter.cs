namespace TechnikPortfolio
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using RestSharp.Extensions;

    public class IssueFilter
    {
        public IReadOnlyList<Issue> FilterIssues(IReadOnlyList<Issue> issues, LevelRange levelRange, string keywordFilter, Collection<Classification> classifications)
        {
            issues = FilterByLevel(issues, levelRange);
            issues = FilterByClassifications(issues, classifications);
            issues = FilterByRootMatchingKeywords(issues, levelRange, keywordFilter);

            RemoveLinksPointingToFilteredIssues(issues);

            return issues;
        }

        private static IReadOnlyList<Issue> FilterByLevel(IReadOnlyList<Issue> issues, LevelRange levelRange)
        {
            issues = issues.Where(i => i.Level >= levelRange.From && i.Level <= levelRange.To).ToList();
            return issues;
        }

        private static IReadOnlyList<Issue> FilterByClassifications(IReadOnlyList<Issue> issues, Collection<Classification> classifications)
        {
            issues = issues.Where(i => classifications.Contains(i.Classification)).ToList();
            return issues;
        }

        private static void RemoveLinksPointingToFilteredIssues(IReadOnlyList<Issue> issues)
        {
            var allIds = issues.Select(x => x.Id).ToList();
            for (int i = 0; i < issues.Count; i++)
            {
                issues[i].LinksTo = issues[i].LinksTo.Where(l => allIds.Contains(l));
            }
        }

        private static IReadOnlyList<Issue> FilterByRootMatchingKeywords(
            IReadOnlyList<Issue> issues,
            LevelRange levelRange,
            string keywordFilter)
        {
            if (!string.IsNullOrWhiteSpace(keywordFilter))
            {
                issues = issues.Where(i => IsInHierarchy(issues, i, keywordFilter, levelRange.From)).ToList();
            }
            return issues;
        }

        private static bool IsInHierarchy(IReadOnlyList<Issue> all, Issue issue, string keywordFilter, Level topLevel)
        {
            if (issue.Level == topLevel)
            {
                return issue.Name.Matches(keywordFilter);
            }

            return issue.LinksTo
                .Where(l => all.Any(x => x.Id == l))
                .Select(l => IsInHierarchy(all, all.Single(i => i.Id == l), keywordFilter, topLevel))
                .Aggregate(false, (a, v) => a || v);
        }
    }
}