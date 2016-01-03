/// <reference path="angular/angular.js" />
/// <reference path="angular/angular.min.js" />
/// <reference path="travelmap/travelmap.js" />
/// <reference path="travelmap/travelmap.min.js" />
(function () {

	"use strict"
	angular.module("app-trips")
	.controller("tripEditorController", tripEditorController);

	function tripEditorController($routeParams, $http) {
		var vm = this;
		vm.tripName = $routeParams.tripName;
		vm.stops = [];
		vm.errorMessage = "";
		vm.isBusy = true;
		vm.newStop = {};

		var url = "/api/trips/" + vm.tripName + "/stops"
		$http.get(url)
		.then(function (response) {
			angular.copy(response.data, vm.stops);
			_showMap(vm.stops);
		}, function (error) {
			vm.errorMessage = "Failed to load stops";
		}).finally(function () {

			vm.isBusy = false;
		})

		vm.addStop = function () {
			vm.isBusy = true;
			debugger;
			$http.post(url, vm.newStop)
			.then(function (response) {
				vm.stops.push(response.data);
				_showMap(vm.stops);
				vm.newStop = {};
			},function(error){
				vm.errorMessage = "Failed to add new stop" + error.text;
			}).finally(function () {
				vm.isBusy = false;
			});
		}

	}

	function _showMap(stops) {
		if (stops && stops.length > 0) {
			var mapStops = _.map(stops, function (item) {
				return {
					lat: item.latitude,
					long: item.longitude,
					info: item.name
				}
			});
			travelMap.createMap({
				stops: mapStops,
				selector: "#map",
				currrentStop: 1,
				initialZoom: 5
			});
		}
	}


})();