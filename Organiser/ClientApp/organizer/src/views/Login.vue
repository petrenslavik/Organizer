﻿<template>
    <div class="container">
        <div class="card border-dark mb-3 rounded col-6 pl-0 pr-0 mr-auto ml-auto">
            <div class="card-header text-center">Login</div>
            <div class="card-body">
                <form novalidate @submit.prevent="login">
                    <div class="input-group mb-3">
                        <input name="Login" required type="email" class="form-control" placeholder="Email..." v-model="Login">
                        <div class="invalid-feedback">
                            Please enter a valid email.
                        </div>
                    </div>
                    <div class="form-group">
                        <input name="Password" required type="password" class="form-control" placeholder="Password..." v-model="Password" >
                        <div class="invalid-feedback">
                            Password should contain at least 8 symbols.
                        </div>
                    </div>
                    <div class="form-group">
                        <input type="hidden" class="form-control is-invalid">
                        <div class="invalid-feedback">
                            {{errorMessage}}
                        </div>
                    </div>
                    <div class="row">
                        <router-link class="col text-center" to="/">
                            <button class="w-50 btn btn-dark">Back</button>
                        </router-link>
                        <div class="col text-center">
                            <button class="w-50 btn btn-dark" type="submit">Login</button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: "Login",
        data: function () {
            return {
                Login: null,
                Password: null,
                errorMessage: null,
            }
        },
        methods: {
            login: function (event) {
                let formData = new FormData(event.target);
                this.$store.dispatch("account/Login", formData, { root: true })
                    .then(() => this.successfulLogin())
                    .catch((error) => this.unsuccessfulLogin(error));
            },
            successfulLogin: function () {
                this.errorMessage = null;
                this.$router.push("/HelloWorld");
            },
            unsuccessfulLogin: function (error) {                
                this.errorMessage = error.response.data;
            }
        }
    }
</script>

<style scoped>
    .container {
        padding-top: 100px;
        padding-bottom: 100px;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        height: 100%;
    }

    label {
        display: block;
    }

    .card {
        border-width: 2px;
        padding-right: 0;
        padding-left: 0;
    }

    .col-6 {
        flex: none;
    }
</style>