import axios from 'axios';

const api = axios.create();
api.defaults.headers.post['Content-Type'] = 'multipart/form-data';

let baseUrl = 'http://localhost:56000/api/account';

export default {
login: function (formData) {
    return axios.post(baseUrl + '/token', formData).then((response) => {
        if(response.statusText == 'OK') {
            const responseData = response.data;
            sessionStorage.setItem('USERNAME', responseData.username);
            sessionStorage.setItem('TOKEN', responseData.access_token);
            console.log(responseData.access_token);
            console.log(axios.defaults.headers.common['Authorization']? axios.defaults.headers.common['Authorization']: 'null');
            console.log(api.defaults.headers.common['Authorization']? api.defaults.headers.common['Authorization'] : 'null');
            //axios.defaults.headers.common['Authorization'] = responseData.access_token;
            api.defaults.headers.common['Authorization'] = responseData.access_token;
        }
        else{
            console.log('penis');
        }
    });
},

logout: function () {
    sessionStorage.removeItem('USERNAME');
    //axios.defaults.headers.common['Authorization'] = '';
    api.defaults.headers.common['Authorization'] = '';
}
};