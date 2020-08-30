import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'
import Login from '../views/Login.vue'
import HelloWorld from '../components/HelloWorld.vue'
import { store } from '@/stores/store';

Vue.use(VueRouter)

const routes = [
    {
        path: '/',
        name: 'Home',
        component: Home
    },
    {
        path: '/login',
        name: 'Login',
        component: Login
    },
    {
        path: '/helloWorld',
        name: 'HelloWorld',
        component: HelloWorld,
        meta: {
            requiresAuth: true
        }
    },
    {
        path: '/about',
        name: 'About',
        // route level code-splitting
        // this generates a separate chunk (about.[hash].js) for this route
        // which is lazy-loaded when the route is visited.
        component: () => import(/* webpackChunkName: "about" */ '../views/About.vue')
    }
]

const router = new VueRouter({
    mode: 'history',
    base: process.env.BASE_URL,
    routes
})


router.beforeEach((to, from, next) => {
    if (to.matched.some(record => record.meta.requiresAuth)) {
        if (!store.getters['account/isAuthenticated']) {
            next({
                path: '/login'
                //query: { redirect: to.fullPath }
            })
        }
        else {
            next();
        }
    } else {
        next();
    }
});


export default router