"use strict";

var publicKey = CryptoJS.enc.Utf8.parse('8080808080808080');
var iv = CryptoJS.enc.Utf8.parse('8080808080808080');

function SetNewPublicKey(newPublicKey) {
    publicKey = newPublicKey;
}

 //function InitiatePublicKey()
 //{
 //    $.ajax({
 //        url: getBaseUrl() + '/Home/GetEncryptionPublicKey',
 //        type: 'GET',
 //        success: function (response) {
 //            publicKey = CryptoJS.enc.Utf8.parse(response);
 //        }
 //    });
 //}
var TrailsSecurity = {
    Encrypt: function (str) {
        var encryptStr = CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(str), publicKey, {
            keySize: publicKey.sigBytes,
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });
        return encryptStr;
    }
};
