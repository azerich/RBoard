'use strict';

function CheckUserName(object) {
        var username = object.value;
        if (username.length >= 5) {
                // id="UserNameSign"
                //begin
                var userNameSign = document.getElementById('UserNameSign');

                userNameSign.classList.remove('text-danger');
                userNameSign.classList.add('text-success');
                //end

                // id="UserName"
                //begin
                var userName = document.getElementById('UserName');

                userName.classList.remove('is-invalid');
                userName.classList.add('is-valid');
                //end
        } else {
                        // id = "UserNameSign"
                        //begin
                        var userNameSign = document.getElementById('UserNameSign');
                        userNameSign.classList.add('text-danger');
                        userNameSign.classList.remove('text-success');
                        //end

                        // id= "UserName"
                        //begin
                        var userName = document.getElementById('UserName');
                        userName.classList.add('is-invalid');
                        userName.classList.remove('is-valid');
                        //end
                }
}

