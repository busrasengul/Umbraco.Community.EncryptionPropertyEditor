angular.module('umbraco.resources').factory('EncryptionPropertyEditorResource', function ($q, $http, umbRequestHelper) {

  return {

    decrypt: function (hash, pw, value, key, iv) {
      if (value.length > 0 && (hash === false || hash == 0)) {
        return umbRequestHelper.resourcePromise(
          $http.get(`${Umbraco.Sys.ServerVariables.BCodes.EncryptionApi}Decrypt?pw=${pw}&stringData=${value}&key=${key}&iv=${iv}`),
          "Failed");
      }
    },

    encrypt: function (hash, salt, pw, plaintext, key, iv) {
      console.debug(`function ${hash} ${plaintext}`);
      if (!plaintext || plaintext.length < 1) return;

      if (hash === true || hash == 1) {
        return umbRequestHelper.resourcePromise(
          $http.get(`${Umbraco.Sys.ServerVariables.BCodes.EncryptionApi}Hash?pw=${pw}&password=${plaintext}&salt=${salt}`),
          "Failed");
      }  
      else {          
          return umbRequestHelper.resourcePromise(
              $http.get(`${Umbraco.Sys.ServerVariables.BCodes.EncryptionApi}Encrypt?pw=${pw}&stringData=${plaintext}&key=${key}&iv=${iv}`),
              "Failed");
      }        
    }
  }
});
