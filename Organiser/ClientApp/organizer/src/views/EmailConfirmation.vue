<template>
    <div></div>
</template>

<script>
    export default {
        name: "EmailConformation",        
        created: function () {
            var token = this.$route.query.token;
            var username = this.$route.query.username;
            this.$store.dispatch("account/ConfirmRegistration", { token, username }, { root: true }).
                then(() => this.emailConfirmed());
        },
        methods: {
            emailConfirmed: function () {
                this.$router.push('/login');
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