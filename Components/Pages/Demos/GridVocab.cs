// 2026-10-25: helper for the Grid overview's Documentation sections (DocSections): a section header shows how many parameters it holds.
using FlexCore.Showcase.Components.Shared;

namespace FlexCore.Showcase.Components.Pages.Demos;

public static partial class GridVocab
{
    public static string Count(params ApiRow[][] arrays) => $"{arrays.Sum(a => a.Length)} parameters";
}
