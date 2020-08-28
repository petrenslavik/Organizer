import axios from 'axios'

const api = axios.create();
api.defaults.headers.post['Content-Type'] = 'multipart/form-data';

const state = {
    currentUser: localStorage.getItem('USER') || null,
    token: localStorage.getItem('TOKEN') || null,
};

const getters = {
    currentUser: state => {
        return state.currentUser;
    },
    isAuthenticated: state => {
        return state.currentUser != null;
    }
};

const mutations = {
    SET_AUTHENTICATION_TOKEN: (state, data) => {
        state.token = data.access_token;
        state.currentUser = data.user;
        localStorage.setItem('TOKEN', state.token);
        localStorage.setItem('USER', state.currentUser);
        axios.defaults.headers.common['Authorization'] = "Bearer " + data.access_token;
        api.defaults.headers.common['Authorization'] = "Bearer " + data.access_token;
    },
    UNSET_AUTHENTICATION_TOKEN: (state) => {
        state.token = null;
        state.currentUser = null;
        localStorage.removeItem('TOKEN');
        localStorage.removeItem('USER');
        delete axios.defaults.headers.common['Authorization'];
        delete api.defaults.headers.common['Authorization'];
    }
};

const actions = {
    Login: async (context, payload) => {
        return api.post("/api/account/token", payload).then((response) => context.commit('account/SET_AUTHENTICATION_TOKEN', response.data, { root: true }));
    },
    Logout: async (context) => {
        return context.commit('account/UNSET_AUTHENTICATION_TOKEN', {}, { root: true });
    },
    Register: async (context, payload) => {
        return api.post("/api/account/register", payload);
    },
    ConfirmRegistration: async (context, payload) => {
        return api.get("/api/account/confirmRegistration?token=" + payload);
    }
};

export default {
    namespaced: true,
    state,
    getters,
    mutations,
    actions,
};