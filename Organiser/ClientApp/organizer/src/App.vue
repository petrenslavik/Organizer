<template>
    <div id="app">
        <nav class="navbar navbar-expand-lg navbar-light bg-light">
            <a class="navbar-brand" href="#">Navbar</a>
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarColor03" aria-controls="navbarColor03" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>

            <div class="collapse navbar-collapse" id="navbarColor03">
                <ul class="navbar-nav mr-auto">
                    <li class="nav-item active">
                        <a class="nav-link" href="#">Home <span class="sr-only">(current)</span></a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">Features</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">Pricing</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">About</a>
                    </li>
                    <li class="nav-item dropdown">
                        <a class="nav-link dropdown-toggle" data-toggle="dropdown" href="#" role="button" aria-haspopup="true" aria-expanded="false">Dropdown</a>
                        <div class="dropdown-menu">
                            <a class="dropdown-item" href="#">Action</a>
                            <a class="dropdown-item" href="#">Another action</a>
                            <a class="dropdown-item" href="#">Something else here</a>
                            <div class="dropdown-divider"></div>
                            <a class="dropdown-item" href="#">Separated link</a>
                        </div>
                    </li>
                </ul>
                <form v-if="isAuthenticated" class="form-inline my-2 my-lg-0">
                    <input class="form-control mr-sm-2" type="text" placeholder="Search">
                    <button class="btn btn-secondary my-2 my-sm-0" type="submit">Search</button>
                </form>
                <button v-if="!isAuthenticated" class="btn btn-secondary my-2 my-sm-0 ml-2" v-on:click="$router.push('Login')">Login</button>
                <form  class="form-inline my-2 my-lg-0 ml-2" v-if="isAuthenticated" novalidate @submit.prevent="logout">
                    <button class="btn btn-secondary my-2 my-sm-0" type="submit">Logout</button>
                </form>
            </div>
        </nav>
        <!--<div id="nav">
          <router-link to="/">Home</router-link> |
          <router-link to="/about">About</router-link>
        </div>-->
        <router-view />
    </div>
</template>
<script>
    import { mapGetters } from 'vuex';

    export default {
        computed: {
            ...mapGetters({              
                isAuthenticated: 'account/isAuthenticated'
            })
        },
        methods: {
            logout: function () {
                this.$store.dispatch("account/Logout", { root: true })
                    .then(() => this.successfulLogout())
                    .catch((error) => this.unsuccessfulLogout(error));
            },
            successfulLogout: function () {
                this.errorMessage = null;
                this.$router.push("/login");
            },
            unsuccessfulLogout: function (data) {
                let str = "";
                for (let property in data.response) {
                    if (Object.prototype.hasOwnProperty.call(data.response, property) && property != "code") {
                        str += data.reponse[property] + '\n';
                    }
                }
                this.errorMessage = str;
            }
        }
    }
</script>

<style>
    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: #2c3e50;
    }

    #nav {
        padding: 30px;
    }

        #nav a {
            font-weight: bold;
            color: #2c3e50;
        }

            #nav a.router-link-exact-active {
                color: #42b983;
            }
</style>
