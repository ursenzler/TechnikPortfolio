namespace TechnikPortfolio
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text.RegularExpressions;

    using RestSharp.Extensions;

    public class IssueFilter
    {
        public IReadOnlyList<Issue> FilterIssues(IReadOnlyList<Issue> issues, LevelRange levelRange, string filter, Collection<Classification> classifications)
        {
            issues = FilterByLevel(issues, levelRange);
            issues = FilterByMatchingFilter(issues, filter);
            issues = FilterByClassifications(issues, classifications);
            
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

        private static IReadOnlyList<Issue> FilterByMatchingFilter(
            IReadOnlyList<Issue> issues,
            string filter)
        {
            var regex = new Regex($".*{filter}.*", RegexOptions.IgnoreCase);

            return issues.Where(issue =>
                {
                    var allRelatedIssues = GetAllRelatedIssues(issue, issues).ToList();
                    var match = allRelatedIssues.Any(i => regex.IsMatch(i.Name));
                    return match;
                }).ToList();
        }
        
        private static IEnumerable<Issue> GetAllRelatedIssues(Issue issue, IReadOnlyList<Issue> all)
        {
            return GetAllRelatedIssuesUpwards(issue, all).Union(GetAllRelatedIssuesDownwards(issue, all));
        }

        private static IEnumerable<Issue> GetAllRelatedIssuesUpwards(Issue issue, IReadOnlyList<Issue> all)
        {
            var result = new List<Issue>();

            foreach (var id in issue.LinksTo)
            {
                var parent = all.Single(x => x.Id == id);

                result.Add(parent);

                foreach (var ancestor in GetAllRelatedIssuesUpwards(parent, all))
                {
                    result.Add(ancestor);
                }
            }

            return result;
        }

        private static IEnumerable<Issue> GetAllRelatedIssuesDownwards(Issue issue, IReadOnlyList<Issue> all)
        {
            var children = all.Where(x => x.LinksTo.Contains(issue.Id));

            foreach (var child in children)
            {
                yield return child;

                foreach (var descendant in GetAllRelatedIssuesDownwards(child, all))
                {
                    yield return descendant;
                }
            }
        }
    }
}