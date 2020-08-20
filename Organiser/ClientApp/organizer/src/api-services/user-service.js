﻿import Axios from 'axios';

let baseUrl: string = 'http://localhost:56000/api/sampledata';

export default {
    getAll() {
        return Axios.get(baseUrl);
    },

    get(id: number) {
        return Axios.get(baseUrl + `/getuser/?id=${id}`);
    }
};