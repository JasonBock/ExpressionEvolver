// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA2210:AssembliesShouldHaveValidStrongNames", Justification = "Sample application.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "DataVisualizationDemosSL" ,Justification = "Silverlight-specific class needs a unique namespace.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "WPF", Justification = "Conventional use.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "WPF", Scope = "namespace", Target = "DataVisualizationDemosWPF", Justification = "Conventional use.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "Delay", Justification = "Namespace for private helpers.")]

// XAML-based warnings
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "DataVisualizationDemos.JellyCharting.#ChartTemplate", Justification = "Used by StaticResource.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "DataVisualizationDemos.JellyCharting.#ColumnChartScale", Justification = "Used by Storyboard.TargetName.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "DataVisualizationDemos.JellyCharting.#LineChartScale", Justification = "Used by Storyboard.TargetName.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope = "member", Target = "DataVisualizationDemos.ChartingIntroduction.#System.Windows.Markup.IComponentConnector.Connect(System.Int32,System.Object)", Justification = "Generated code.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope = "member", Target = "DataVisualizationDemos.ColumnsWithColor.#System.Windows.Markup.IComponentConnector.Connect(System.Int32,System.Object)", Justification = "Generated code.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope = "member", Target = "DataVisualizationDemos.DynamicPieGradients.#System.Windows.Markup.IComponentConnector.Connect(System.Int32,System.Object)", Justification = "Generated code.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope = "member", Target = "DataVisualizationDemos.ChartingIntroduction.#Container", Justification = "Used by Silverlight project.")]
