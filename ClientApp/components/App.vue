<template>
<div>
  <div class="container-fluid m-3 p-3">
        <template v-if="$route.matched.length">
          <transition name="fade" modd="out-in">
            <router-view></router-view>
		      </transition>
        </template>
  </div>
</div>

</template>

<script>
export default {
  data: function () {
      return {
        authenticated: false,
        userInfo: null
      }
  },
  computed: {
      loggedIn () {
        return this.$store.state.loggedIn
      }
    },
  methods: {
    async checkAuthentication () {
      return this.$store.state.loggedIn
    }
  }
}
</script>
