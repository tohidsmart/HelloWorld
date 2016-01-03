/// <reference path="angular/angular.js" />
/// <reference path="angular/angular.min.js" />

(function () {

	"use strict";

	//Getting the existing module
	angular.module("app-trips")
	.controller("tripsController", tripsController);

	function tripsController($http) {

		var vm = this;

		vm.trips = [];
		vm.newTrip = {};
		vm.errorMessage = "";
		vm.isBusy = true;

		//GET
		$http.get("/api/trips").then(function (response) {
			angular.copy(response.data, vm.trips)
		}, function (error) {
			vm.errorMessage = "Failed to load data" + error;
		}).finally(function () {
			vm.isBusy = false;
		});

		//POST
		vm.addTrip = function () {
			vm.isBusy = true;
			$http.post("/api/trips", vm.newTrip)
			.then(function (response) {
				vm.trips.push(response.data);
				vm.newTrip = {};
			}, function (error) {
				vm.errorMessage = "Failed to save the new trip"+error;
			}
			).finally(function () {
				vm.isBusy = false;
			})
		}

	}

})();

