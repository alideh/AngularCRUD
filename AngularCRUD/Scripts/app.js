var TutorApp = angular.module("TutorApp", ["ngResource"]).
    config(function($routeProvider) {
        $routeProvider.
            when('/', { controller: ListCtrl, templateUrl: 'list.html' }).
            when('/new', { controller: CreateCtrl, templateUrl: 'details.html' }).
            when('/edit/:editId', { controller: EditCtrl, templateUrl: 'details.html' }).
            otherwise({ redirectTo: '/' });
    })
    .directive('greet', function() {
        return {
            template: '<h2> Greeting from {{from}} to my dear {{to}}</h2>',
            controller: function($scope, $element, $attrs) {
                $scope.from = $attrs.from;
                $scope.to = $attrs.greet;
            }
        };
    });
TutorApp.factory('Tutor', function ($resource) {
    return $resource('/api/Tutor/:id',{id:'@id'},{update: {method: 'PUT'}});
});

var EditCtrl = function($scope, $location, $routeParams, Tutor) {
    var id = $routeParams.editId;
    $scope.item = Tutor.get({ id: id });
    $scope.action = "Update";

    $scope.save = function() {
        Tutor.update({ id: id }, $scope.item, function() {
            $location.path('/');
        });
    };
};

var CreateCtrl = function($scope, $location, Tutor) {
    $scope.save = function () {
        
        Tutor.save($scope.item, function ()
        {
            $location.path('/');
        });
    };


};

var ListCtrl = function($scope, $location, Tutor) {
    $scope.action = "Add";
    $scope.search = function() {
        Tutor.query({
                q: $scope.query,
                sort: $scope.sort_order,
                desc: $scope.is_desc,
                offset: $scope.offset,
                limit: $scope.limit
            },
            function(data) {
                $scope.more = data.length === 20;
                $scope.tutors = $scope.tutors.concat(data);
            });
    };

    $scope.sort = function(col) {
        if ($scope.sort_order === col) {
            $scope.is_desc = !$scope.is_desc;
        } else {
            $scope.sort_order = col;
            $scope.is_desc = false;
        }
        $scope.reset();
    };

    $scope.show_more = function() {
        $scope.offset += $scope.limit;
        $scope.search();
    };
    $scope.has_more = function() {
        return $scope.more;
    };

    $scope.reset = function() {
        $scope.limit = 20;
        $scope.offset = 0;
        $scope.tutors = [];
        $scope.more = true;
        $scope.search();
    };

    $scope.delete = function() {
        var id = this.tutor.Id;
        Tutor.delete({ id: id }, function () {
            $('#tutor_'+id).fadeOut();
        });
    };
    $scope.sort_order = "FullName";
    $scope.is_desc = false;
  
    $scope.reset();
}