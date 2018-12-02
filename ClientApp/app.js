import Vue from 'vue'
import App from './components/App'
import router from './router'
import store from './store'
import { sync } from 'vuex-router-sync'
import 'bootstrap';
import 'bootstrap/dist/css/bootstrap.min.css'

// Sync Vue router and the Vuex store
sync(store, router)

new Vue({
  el: '#app',
  store,
  router,
  template: '<App/>',
  components: { App }
})

store.dispatch('checkLoggedIn')
