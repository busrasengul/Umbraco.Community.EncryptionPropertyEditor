angular.module("umbraco").controller("EncryptionPropertyEditorController",
  ["$scope",
   "encryptionPropertyEditorResource",
  function ($scope, EncryptionPropertyEditorResource) {
    console.log($scope.model);
    $scope.getAes = function () {
      var key = $scope.model.config.key;
      var iv = $scope.model.config.iv;
    EncryptionPropertyEditorResource.getAes(key, iv).then(function (response) {
      if (response) {
      } else {
        return false;
      }
    });
  }

    $scope.encrypt = function () {
      var hash = $scope.model.config.useHash;
      var pw = $scope.model.config.password;
      var value = $scope.model.value;
      EncryptionPropertyEditorResource.encrypt(hash, pw, value);
  }

    $scope.decrypt = function () {

      var hash = $scope.model.config.useHash;
      var salt = $scope.model.config.salt;
      var pw = $scope.model.config.password;
      var plaintext = $scope.model.plaintext;
      EncryptionPropertyEditorResource.decrypt(hash, salt, pw, plaintext).then(function (response) {
      if (response) {
        plaintext = response.data.replace(/"/g, "");
      } else {
        return false;
      }
    });
  }
}]);
