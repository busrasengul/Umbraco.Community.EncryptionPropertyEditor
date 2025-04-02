angular.module("umbraco").controller("EncryptionPropertyEditorController",
  ["$scope", "EncryptionPropertyEditorResource",
  function ($scope, EncryptionPropertyEditorResource) {
    console.log($scope.model);
    var key = $scope.model.config.key;
    var iv = $scope.model.config.iv;
    var salt = $scope.model.config.salt;
    var hash = $scope.model.config.useHash;
    var pw = $scope.model.config.password;
    var plaintext = $scope.model.plaintext;
    console.log(plaintext);


    $scope.getAes = function () {
    EncryptionPropertyEditorResource.getAes(key, iv).then(function (response) {
      if (response) {
        console.log(response);
      } else {
        return false;
      }
    });
  }

    $scope.encrypt = function () {
      console.log("encrypt");
      EncryptionPropertyEditorResource.encrypt(hash, salt, pw, plaintext, key, iv);
  }

    $scope.decrypt = function () {
      EncryptionPropertyEditorResource.decrypt(hash, salt, pw, plaintext).then(function (response) {
      if (response) {
        plaintext = response.data.replace(/"/g, "");
      } else {
        return false;
      }
    });
  }
}]);
