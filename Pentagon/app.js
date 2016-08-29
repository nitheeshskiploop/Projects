/**
 * Created by Edison on 22-08-2016.
 */

(function () {
    'use strict';
    var uid, tid;
    var app = angular.module('tutorialApp', ['routes'])

        .controller('loginCtrl', function ($scope, $http, $location) {
            $scope.username = '';
            $scope.password = '';
            $scope.loginerror = false;
            $scope.login = function () {
                $scope.loginerror = false;
                $http.get('/site/login', { params: { username: $scope.username, password: $scope.password } })
                    .then(function (userid) {
                        console.log(userid.data);
                        if (userid.data) {
                            uid = userid.data;
                            $location.path('panel');
                        }
                        else $scope.loginerror = true;
                    }, function (data) {
                        console.log("Error");
                    });
            };
        })

        .controller('panelCtrl', function ($scope, $http, $location) {
            $scope.name = "";
            $scope.showModal = false;
            //  $scope.courses = ['C Basics', 'C++ Concepts', 'Java Programming', 'Android App Development', 'HTML5', 'CSS3', 'JQuery', 'Angular JS', 'Bootstrap', 'Knockout JS', 'Node JS', 'INK Framework', 'Materialize CSS'];
            $scope.courses = ['Loading..'];

            $http.get('site/listofcourses?userid=' + uid)
            .then(function (data) {
                $scope.courses = data.data;
            });

            $scope.addTutorial = function () {
                if ($scope.name != '') {
                    $scope.showModal = false;
                    $http.post('site/addtutorial?uid=' + uid + '&tutorialname=' + $scope.name)
                    .then(function (data) {
                        $scope.courses.push(data.data);
                    });
                }
                $scope.showModal = false;
                $scope.name = "";
            }
            $scope.deleteCourse = function (index) {
                $scope.courses.splice(index, 1);
            };

            $scope.setCourseId = function (index) {
                tid = index;
                $location.path('chapters');
            };
        })

        .controller('chapterCtrl', function ($scope, $http) {
            //$scope.list = ['Preface', 'Introduction', 'Data Types', 'Functions', 'Object Orientation']
            $scope.chapters = [];
            $scope.name = '';
            $scope.cid = 0;
            $scope.isViewEnabled = true;
            $scope.showModal = false;

            $scope.toggleEditView = function () {

                if ($scope.isViewEnabled == false)
                    $http.post('site/updatechapter', $scope.chapters[$scope.cid]);
                $scope.isViewEnabled = !$scope.isViewEnabled;
            };

            $http.get('site/getchapters?tutorialid=' + tid)
            .then(function (data) {
                $scope.chapters = data.data;
            });

            $scope.setCid = function (index) {
                $scope.cid = index;
            };

            $scope.addChapter = function () {
                if ($scope.name != '')
                {
                    $scope.showModal = false;
                    $http.get('site/addchapter?tid=' + tid + '&chaptername=' + $scope.name)
                    .then(function (data) {
                        $scope.chapters.push(data.data);
                    });
                }
                $scope.showModal = false;
                $scope.name = "";
            }

        });

})();