<template>
        <div class="user-pins">
            <navbar></navbar>
            <div class="card-columns">
                <div v-for="userpin in userpins">
                    <pins :pin='userpin'></pins>
                </div>
            </div>
        </div>
    </template>
    <script>
        import Router from '../router'
        import Navbar from './Navbar'
        import Pins from './Pin'
        export default {
            name: 'UserPins',
            data() {
                return {
                }
            },
            mounted() {
                this.$store.dispatch("getUserPins");
            },
            watch: {
                userpins: function(value) {
                    this.$store.dispatch("getUserPins");
                }
            },
            computed: {
                userpins() {
                    return this.$store.state.userPins;
                }
            },
            components: {
                Navbar,
                Pins
            }
        }
    
    </script>
    <style scoped>
    </style>