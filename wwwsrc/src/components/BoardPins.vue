<template>
    <div class="user-pins">
        <navbar></navbar>
        <div class="card-columns">
            <div v-for="boardpin in boardpins">
                <pins :pin='boardpin'></pins>
            </div>
        </div>
    </div>
</template>
<script>
    import Router from '../router'
    import Navbar from './Navbar'
    import Pins from './Pin'
    export default {
        name: 'BoardPins',
        data() {
            return {
            }
        },
        mounted() {
            this.$store.dispatch("getBoardPins", this.$route.params.boardId);
        },
        methods: {

        },
        computed: {
            boardpins() {
                return this.$store.state.boardPins;
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