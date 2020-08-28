import axios from 'axios';
import account from '@/stores/account'

let baseUrl = '/api/sampledata';

const api = axios.create();

axios.defaults.headers.common['Authorization'] = "Bearer " + account.state.token;
api.defaults.headers.common['Authorization'] = "Bearer " + account.state.token;

export default {
    getAll: function () {
        return axios.get(baseUrl);
    },

    get: function (id) {
        return axios.get(baseUrl + `/getuser/?id=${id}`);
    }
};