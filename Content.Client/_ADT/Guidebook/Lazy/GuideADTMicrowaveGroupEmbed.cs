using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Content.Client.Guidebook.Controls;
using Content.Client.Guidebook.Richtext;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Kitchen;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Shared.Prototypes;

namespace Content.Client._ADT.Guidebook.Lazy;

[UsedImplicitly]
public sealed class GuideADTMicrowaveGroupEmbed : ADTLazyList, IDocumentTag
{
    private const float EstimatedHeight = 140f;
    // Wormix-Changes: vanilla recipes default to MicrowaveRecipeType.Microwave, so it is part of the active set
    private const int ActiveTypes = (int) (MicrowaveRecipeType.Microwave | MicrowaveRecipeType.ADTMicrowave | MicrowaveRecipeType.ADTAssembler | MicrowaveRecipeType.MedicalAssembler | MicrowaveRecipeType.TribalOven);

    [Dependency] private IPrototypeManager _prototype = default!;

    public GuideADTMicrowaveGroupEmbed()
    {
        IoCManager.InjectDependencies(this);
    }

    public bool TryParseTag(Dictionary<string, string> args, [NotNullWhen(true)] out Control? control)
    {
        control = null;
        if (!args.TryGetValue("Group", out var group))
            return false;

        var recipes = new List<(MicrowaveMealRecipePrototype Recipe, EntityPrototype Result)>();
        foreach (var recipe in _prototype.EnumeratePrototypes<MicrowaveMealRecipePrototype>())
        {
            if (!recipe.Group.Equals(group)
                || recipe.HideInGuidebook
                || (recipe.RecipeType & ActiveTypes) == 0
                || !_prototype.TryIndex(recipe.Result, out var result))
            {
                continue;
            }

            recipes.Add((recipe, result));
        }

        foreach (var (recipe, result) in recipes.OrderBy(pair => pair.Result.Name))
        {
            var target = recipe;
            AddLazy(() => new GuideMicrowaveEmbed(target), GetSearchText(recipe, result), result, EstimatedHeight);
        }

        control = this;
        return true;
    }

    private string GetSearchText(MicrowaveMealRecipePrototype recipe, EntityPrototype result)
    {
        var text = new StringBuilder(result.Name);
        foreach (var solid in recipe.Ingredients.Solids.Keys)
        {
            if (_prototype.TryIndex(solid, out var entity))
                text.Append(' ').Append(entity.Name);
        }

        foreach (var reagent in recipe.Ingredients.Reagents.Keys)
        {
            if (_prototype.TryIndex<ReagentPrototype>(reagent, out var prototype))
                text.Append(' ').Append(prototype.LocalizedName);
        }

        return text.ToString();
    }
}
