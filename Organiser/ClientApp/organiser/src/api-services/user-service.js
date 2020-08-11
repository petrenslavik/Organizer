import axios from 'axios';

let baseUrl = 'http://127.0.0.1:56000/api/sampledata';
const api = axios.create();

export default {
getAll: function() {
    axios.defaults.headers.get['Authorization'] = 'Bearer ' + sessionStorage.getItem('TOKEN');
    return axios.get(baseUrl);
},

get: function(id) {
    api.defaults.headers.get['Authorization'] = 'Bearer ' + sessionStorage.getItem('TOKEN');
    return axios.get(baseUrl + `/getuser/?id=${id}`);
}
};