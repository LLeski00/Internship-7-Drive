using Drive.Presentation.Extensions;
using Drive.Presentation.Factories;

var loginMenuActions = LoginMenuFactory.CreateActions();
loginMenuActions.PrintActionsAndOpen();