

function getAccessToken() {
    if (location.hash) {
        if (location.hash.split('access_token=')) {
            var accessToken = location.hash.split('access_token=')[1].split('&')[0];
            if (accessToken) {
                isUserRegistered(accessToken);
            }
        }
    }
}

function isUserRegistered(accessToken) {
    $.ajax({
        url: '/api/Account/UserInfo',
        method: 'get',
        header: {
            'content-type': 'application/JSON',
            'Authorization':'Bearer ' + accessToken
        },
        success: function (response) {
            if (response.HasRegistered) {
                localStorage.setItem('accessToken', accessToken);
                localStorage.setItem('username', response.Email);
                window.location.href = "Data.html";
            }
            else {
                signupExternalUser(accessToken,response.LoginProvider);
            }
        }
    });
}

function signupExternalUser(accessToken,provider) {

    $.ajax({
        url: '/api/Account/RegisterExternal',
        method: 'POST',
        header: {
            'content-type': 'application/JSON',
            'Authorization': 'Bearer ' + accessToken
        },
        success: function (response) {
            window.location.href = "/api/Account/ExternalLogin?provider=" + provider + "&response_type=token&client_id=self&redirect_uri=http%3A%2F%2Flocalhost%3A44312%2FLogin.html&state=wgQ694ZQ79SBX7Mehe1gODykNjm3lKwg5PTVgMH6UyQ1";
        }
    });
  
}