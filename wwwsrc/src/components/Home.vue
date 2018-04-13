<template>
  <div class="home">
    <navbar></navbar>
    <div class="card-columns">
      <div v-for="pin in pins">
        <pins :pin='pin'></pins>
      </div>
    </div>
  </div>
</template>

<script>
  import Navbar from './Navbar'
  import Pins from './Pin'
  export default {
    name: 'Home',
    data() {
      return {
        boardpin: {},
        form: false
      }
    },
    mounted() {
      this.$store.dispatch("getPins");
      this.$store.dispatch("getBoards");
      this.$store.dispatch("authenticate");
    },
    methods: {
      pinViews(pin) {
        this.$store.dispatch("updateViews", { id: pin.id, views: pin.views++ })
      },
      savePin() {
        this.$store.dispatch("savePin", this.boardpin);
      }
    },
    computed: {
      pins() {
        return this.$store.state.pins;
      },
      user() {
        return this.$store.state.user;
      },
      boards() {
        return this.$store.state.boards;
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