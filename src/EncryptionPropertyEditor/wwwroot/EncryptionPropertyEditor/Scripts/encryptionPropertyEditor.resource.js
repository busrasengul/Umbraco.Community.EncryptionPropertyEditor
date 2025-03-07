angular.module('umbraco.resources').factory('encryptionPropertyEditorResource', function ($q, $http, umbRequestHelper) {

  return {
    getAes: function (key, iv) {
      return umbRequestHelper.resourcePromise(
        $http.get("backoffice/EncryptionApi/GetAes?key=" + key + "&iv=" + iv),
        "Failed");
    },

    decrypt: function (hash, pw, value) {
      if (value.length > 0 && (hash === false || hash == 0)) {
        return umbRequestHelper.resourcePromise(
          $http.get("backoffice/EncryptionApi/Decrypt?pw=" + pw + "&stringData=" + value),
          "Failed");
      }
    },

    encrypt: function (hash, salt, pw, plaintext) {
      if (hash === true || hash == 1) {
        if (plaintext.lenght > 0) {
          return umbRequestHelper.resourcePromise(
            $http.get("backoffice/EncryptionApi/Hash?pw=" + pw + "&password=" + plaintext + "&salt" + salt,),
            "Failed");
        }
        else {
          if (plaintext.length > 0) {
            return umbRequestHelper.resourcePromise(
              $http.get("backoffice/EncryptionApi/Encrypt?pw==" + pw + "&stringData=" + plaintext),
              "Failed");
          }
        }
      }
    }
  }
});
