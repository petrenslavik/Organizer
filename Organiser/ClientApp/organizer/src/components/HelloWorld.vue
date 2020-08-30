<template>
    <div class="hello">
        <ul id="example-1">
            <li v-for="user in users" :key="user.id">
                {{ user.Username }}
            </li>
        </ul>
    </div>
</template>

<script>
    import UserService from "@/api-services/user-service"

    export default {
        name: 'HelloWorld',
        props: {
            msg: String
        },
        data: function () {
            return {               
                users: null,
            }
        },
        mounted() {
            UserService.getAll().then((response) => {
                this.users = response.data.users;
            })
        }
    }
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
    h3 {
        margin: 40px 0 0;
    }

    ul {
        list-style-type: none;
        padding: 0;
    }

    li {
        display: inline-block;
        margin: 0 10px;
    }

    a {
        color: #42b983;
    }
</style>
