<template>
    <div class="boards">
        <navbar></navbar>
        <div class="card-columns">
            <div v-for="board in boards">
                <boards :board='board'></boards>
            </div>
        </div>
    </div>
</template>
<script>
    import Router from '../router'
    import Navbar from './Navbar'
    import Boards from './Board'
    export default {
        name: 'UserBoards',
        data() {
            return {
            }
        },
        mounted() {
            this.$store.dispatch("authenticate");
        },
        methods: {

        },
        computed: {
            user() {
                return this.$store.state.user;
            },
            boards() {
                return this.$store.state.boards;
            }
        },
        components: {
            Navbar,
            Boards
        }
    }
</script>
<style scoped>

</style>