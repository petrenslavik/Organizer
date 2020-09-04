<template>
    <div class="container">
        <div class="card border-dark mb-3 rounded col-6 pl-0 pr-0 mr-auto ml-auto">
            <div class="card-header text-center">Register</div>
            <div class="card-body">
                <form novalidate @submit.prevent="register">
                    <div>
                        <div class="form-group mb-0">
                            <input required name="Email" class="form-control" type="email" v-model="Email" placeholder="Enter email..." />
                        </div>

                        <div class="form-group mb-0 mt-2">
                            <input required name="Username" class="form-control" v-model="Username" placeholder="Enter username..." />
                        </div>

                        <div class="form-group mb-0 mt-2">
                            <input required name="Password" class="form-control" type="password" v-model="Password" placeholder="Enter password..." />
                        </div>
                        <!--<div class="error" v-if="!$v.Password.required">Please enter password</div>-->
                        <div class="error" v-if="!$v.Password.minLength">Password must have at least {{$v.Password.$params.minLength.min}} letters.</div>

                        <div class="form-group mb-0 mt-2">
                            <input required name="RepeatPassword" class="form-control" type="password" v-model="RepeatPassword" placeholder="Repeat password..." />
                        </div>
                        <!--<div class="error" v-if="!$v.RepeatPassword.required">Please repeat password</div>-->
                        <div class="error" v-if="!$v.RepeatPassword.sameAsPassword">Passwords doesn`t match</div>
                        <div class="form-group">
                            <input type="hidden" class="form-control is-invalid">
                            <div class="invalid-feedback">
                                {{errorMessage}}
                            </div>
                        </div>
                        <div class="row">
                            <div class="col text-center">
                                <button class="btn btn-block btn-dark" type="submit">Register</button>
                            </div>
                        </div>
                    </div>                   
                </form>
            </div>
        </div>
    </div>
</template>

<script>
    import { required, minLength, sameAs } from 'vuelidate/lib/validators'

    export default {
        name: "Register",
        data: function () {
            return {
                Email: null,
                Username: null,
                Password: null,
                RepeatPassword: null,
                errorMessage: null,
            }
        },
        validations: {
            Password: {
                required,
                minLength: minLength(8)
            },
            RepeatPassword: {
                required,
                minLength: minLength(8),
                sameAsPassword: sameAs('Password')
            }
        },
        methods: {
            register: function (event) {
                let formData = new FormData(event.target);
                this.$store.dispatch("account/Register", formData, { root: true })
                    .then(() => this.successfulRegister())
                    .catch((error) => this.unsuccessfulRegister(error));
            },
            successfulRegister: function () {
                this.errorMessage = null;
                this.$router.push("/registerConfirmationInfo");
            },
            unsuccessfulRegister: function (error) {
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

    .error {
        padding: 0 6px;
        color: red;
        text-align: left;
        font-size: 0.75rem;
    }
</style>