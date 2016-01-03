/// <reference path="angular/angular.js" />
/// <reference path="angular/angular.min.js" />
(function () {

	"use strict"
	angular.module("simpleControls", [])
	.directive("waitCursor", waitCursor);

	function waitCursor() {
		return {
			scope:{
			show:"=displayWhen"
			},
			restrict:"E",
			templateUrl: "/views/WaitCursor.html"
		}
	}


})();