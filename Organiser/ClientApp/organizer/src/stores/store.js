import Vue from 'vue';
import Vuex from 'vuex';
import account from '@/stores/account'

Vue.use(Vuex);

export const store = new Vuex.Store({
    state: {
    },
    getters: {},
    mutations: {
    },
    actions: {},
    modules: {
        account,
    }
});