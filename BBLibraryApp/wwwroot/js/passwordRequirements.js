'use strict';
var currentPath = window.location.pathname;
if (currentPath !== '/identity/account/login' && currentPath !== '/identity/account/manage/deletepersonaldata'
    && (document.querySelector('#Input_Password') || document.querySelector('#Input_NewPassword')
        || document.querySelector('#Password'))) {

    var passwordInput = document.querySelector('#Input_Password');
    if (!passwordInput) {
        passwordInput = document.querySelector('#Input_NewPassword');
    }
    if (!passwordInput) {
        passwordInput = document.querySelector('#Password');
    }

    var upper = document.querySelector('#PasswordUpperDivId');
    var lower = document.querySelector('#PasswordLowerDivId');
    var digit = document.querySelector('#PasswordDigitDivId');
    var special = document.querySelector('#PasswordSpecialDivId');
    var length = document.querySelector('#PasswordLengthDivId');

    // When the user starts to type something inside the password field
    passwordInput.addEventListener('keyup', function () {

        // Validate capital letters
        var upperCaseLetters = /[A-Z]/g;
        if (passwordInput.value.match(upperCaseLetters)) {
            upper.classList.remove('invalid');
            upper.classList.add('valid');
        } else {
            upper.classList.remove('valid');
            upper.classList.add('invalid');
        }

        // Validate lowercase letters
        var lowerCaseLetters = /[a-z]/g;
        if (passwordInput.value.match(lowerCaseLetters)) {
            lower.classList.remove('invalid');
            lower.classList.add('valid');
        } else {
            lower.classList.remove('valid');
            lower.classList.add('invalid');
        }

        // Validate digit
        var numbers = /[0-9]/g;
        if (passwordInput.value.match(numbers)) {
            digit.classList.remove('invalid');
            digit.classList.add('valid');
        } else {
            digit.classList.remove('valid');
            digit.classList.add('invalid');
        }

        // Validate special
        var specials = /\W/g;
        if (passwordInput.value.match(specials)) {
            special.classList.remove('invalid');
            special.classList.add('valid');
        } else {
            special.classList.remove('valid');
            special.classList.add('invalid');
        }

        // Validate length
        if (passwordInput.value.length >= 8) {
            length.classList.remove('invalid');
            length.classList.add('valid');
        } else {
            length.classList.remove('valid');
            length.classList.add('invalid');
        }
    });
}