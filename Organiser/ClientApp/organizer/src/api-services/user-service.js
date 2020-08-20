import Axios from 'axios';

let baseUrl = 'http://localhost:56000/api/sampledata';

export default {
    getAll() {
        return Axios.get(baseUrl);
    },

    get(id) {
        return Axios.get(baseUrl + `/getuser/?id=${id}`);
    }
};