/**
 * Created by Edison on 22-08-2016.
 */

(function () {
    'use strict';
    var app = angular.module('tutorialApp', ['routes'])

        .controller('loginCtrl', function ($scope, $http, $location) {
            $scope.username = '';
            $scope.password = '';
            $scope.loginerror = false;
            $scope.login = function () {
                $scope.loginerror = false;
                $http.get('/site/login', {params : {username : $scope.username, password : $scope.password}})
                    .then(function (userid) {
                        console.log(userid.data);
                        if(userid.data)
                        {
                            $scope.uid = userid.data;
                            $location.path('panel');
                        }
                        else $scope.loginerror = true;
                    },function (data) {
                        console.log("Error");
                    });
            };
        })

        .controller('panelCtrl', function ($scope) {
            $scope.name = "";
            $scope.showModal = false;
            $scope.courses = ['C Basics', 'C++ Concepts', 'Java Programming', 'Android App Development', 'HTML5', 'CSS3', 'JQuery', 'Angular JS', 'Bootstrap', 'Knockout JS', 'Node JS', 'INK Framework', 'Materialize CSS'];
            $scope.addCourse = function () {
                if ($scope.name != '')
                    $scope.courses.push($scope.name);
                $scope.showModal = false;
                $scope.name = "";
            }
            $scope.deleteCourse = function (index) {
                $scope.courses.splice(index, 1);
            }
        })

        .controller('chapterCtrl', function ($scope) {
            $scope.list = ['Preface', 'Introduction', 'Data Types', 'Functions', 'Object Orientation']
            $scope.desc = ['C is a general-purpose, procedural, imperative computer programming language developed in 1972 by Dennis M. Ritchie at the Bell Telephone Laboratories to develop the UNIX operating system. C is the most widely used computer language. It keeps fluctuating at number one scale of popularity along with Java programming language, which is also equally popular and most widely used among modern software programmers.'];

            $scope.isViewEnabled = true;
            $scope.url = 'https://www.youtube.com/watch?v=yET4p-r2TI8';
            
            $scope.toggleEditView = function () {
                $scope.isViewEnabled = ! $scope.isViewEnabled;
            }

        })

})();