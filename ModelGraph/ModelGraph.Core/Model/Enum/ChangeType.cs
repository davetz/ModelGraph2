namespace ModelGraph.Core
{/*
 */
    public enum ChangeType
    {
        NoChange,

        GoToEnd,
        GoToHome,

        ToggleLeft,
        ExpandLeft,
        CollapseLeft,
        ExpandLeftAll,

        ToggleRight,
        ExpandRight,
        CollapseRight,

        ToggleFilter,
        ExpandFilter,
        CollapseFilter,

        FilterSortChanged,
    }
}
