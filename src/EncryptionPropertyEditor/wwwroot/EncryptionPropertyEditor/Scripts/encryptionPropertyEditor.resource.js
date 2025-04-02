angular.module('umbraco.resources').factory('EncryptionPropertyEditorResource', function ($q, $http, umbRequestHelper) {

  return {
    getAes: function (key, iv) {
      return umbRequestHelper.resourcePromise(
        $http.get("~/umbraco/api/EncryptionApi/GetAes?key=" + key + "&iv=" + iv),
        "Failed");
    },

    decrypt: function (hash, pw, value, key, iv) {
      if (value.length > 0 && (hash === false || hash == 0)) {
        return umbRequestHelper.resourcePromise(
          $http.get("~/umbraco/api/EncryptionApi/Decrypt?pw=" + pw + "&stringData=" + value + "&key=" + key + "&iv=" + iv),
          "Failed");
      }
    },

    encrypt: function (hash, salt, pw, plaintext, key, iv) {
      console.log("function");
      console.log(hash);
      console.log(plaintext);
      if (hash === true || hash == 1) {
      
        if (plaintext.lenght > 0) {
          console.log(plaintext);
          return umbRequestHelper.resourcePromise(
            $http.get("~/umbraco/api/EncryptionApi/Hash?pw=" + pw + "&password=" + plaintext + "&salt" + salt),
            "Failed");
        }
        else {
          if (plaintext.length > 0) {
            console.log(plaintext);
            console.log(plaintext);
            return umbRequestHelper.resourcePromise(
              $http.get("~/umbraco/api/EncryptionApi/Encrypt?pw=" + pw + "&stringData=" + plaintext + "&key=" + key + "&iv=" + iv),
              "Failed");
          }
        }
      }
    }
  }
});
