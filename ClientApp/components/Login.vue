<template>
  <div>
    <div class="container">
    <div class="row">
      <div class="col">
        <h2>Login</h2>
        <p v-if="$route.query.redirect">
          You need to login first.
        </p>
        <form @submit.prevent="login" autocomplete="off">
          <div class="form-group">
            <label for="email">Email</label>
            <input id="email" v-model="email" class="form-control" placeholder="you@example.com">
          </div>
          <div class="form-group">
            <label for="password">Password</label>
            <input id="password" v-model="password" class="form-control" placeholder="password" type="password">
          </div>
          <div class="form-group">
          <button type="submit" class="btn btn-primary">login</button>
          <p v-if="loginError" class="error">{{loginError}}</p>
          </div>
        </form>
      </div>
    </div>
  </div>
</div>
</template>

<script>
export default {
  data () {
    return {
      email: '',
      password: '',
      error: false
    }
  },
  computed: {
    loginError () {
      return this.$store.state.loginError
    }
  },
  methods: {
    login () {
      this.$store.dispatch('login', {
        email: this.email,
        password: this.password
       })
    }
  }
}
</script>
