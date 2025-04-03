angular.module("umbraco").controller("EncryptionPropertyEditorController",
  ["$scope", "EncryptionPropertyEditorResource",
    function ($scope, EncryptionPropertyEditorResource) {

      var vm = this;
      var key = $scope.model.config.key;
      var iv = $scope.model.config.iv;
      var salt = $scope.model.config.salt;
      var hash = $scope.model.config.useHash;
      var pw = $scope.model.config.password;
      var plaintext = $scope.model.plaintext;    
      console.log($scope.model.config);
      vm.encrypt = function () {
        if (!$scope.model.plaintext || $scope.model.plaintext.length < 1) return;
        console.debug("encrypt");
        EncryptionPropertyEditorResource.encrypt(hash, salt, pw, $scope.model.plaintext, key, iv).then(function (response) {
          $scope.model.value = response;
        });
      }

      vm.decrypt = decrypt;
      vm.hashing = hash;
      vm.hasValue = function () {
        return $scope.model.value ? true : false;
      }

      console.log($scope.model.value);
      console.log($scope.model.config.useHash);
      if ($scope.model.value) {
        if (!$scope.model.config.useHash) {
          decrypt();
        }
      }

    function decrypt() {
      console.log(`decrypt ${pw}`);
      EncryptionPropertyEditorResource.decrypt(hash, pw, $scope.model.value, key, iv).then(function (response) {
        console.log(response);
        if (response) {
          plaintext = response.replace(/"/g, "");
          $scope.model.plaintext = plaintext;
        } else {
          return false;
        }
      });
    }
}]);
