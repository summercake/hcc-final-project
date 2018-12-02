<template>
  <div>
    <div class="container">
      <div class="row">
        <div class="col">
          <div class="jumbotron jumbotron-fluid p-3">
          <h1 class="display-4">My Private Blog</h1>
            <p class="lead">This is a personal blog project.</p>
            <hr class="my-4">
            <p>Lorem, ipsum dolor sit amet consectetur adipisicing elit. Autem harum reiciendis cum repudiandae, adipisci recusandae facilis ipsam repellendus quibusdam deleniti ratione soluta placeat et. Maiores voluptates vel eos amet dignissimos?</p>
            <router-link to="/logout">Log out</router-link>
          </div>
        </div>
    </div>
  <br>
    <div class="container">
        <div class="row">
          <div class="col">
            <h2>{{name}}, here's blogs list</h2>
          </div>
        </div>
        <div class="row">
          <div class="col">
            <div class="card p-3 my-3" v-for="(blog, index) in blogs" :key="index" :item="blog">
              <div class="media" >
              <h4 class="mr-auto">{{index}} / {{blog.title}}</h4>
              <button class="btn btn-sm btn-danger" @click="deleteBlog(blog.id)">Delete</button>
            </div>
            <div class="card-body">
              <p>{{blog.content}}</p>
            </div>
            </div>
          </div>
        </div>
    </div>
    <br>
    <div class="container">
      <div class="row">
        <div class="col">
          <div class="card p-3">
            <h2>Post New Blog</h2>
            <form @submit.prevent="addBlog" autocomplete="off" id="post-form" class="my-3">
          <div class="form-group">
            <label for="title">Title</label>
            <input id="title" v-model="title" class="form-control" placeholder="Article Title">
          </div>
          <div class="form-group">
            <label for="content">Content</label>
            <textarea id="content" v-model="content" class="form-control" placeholder="Article Content" rows="10"></textarea>
          </div>
          <div class="form-group">
            <button class="btn btn-sm btn-danger">Submit</button>
          </div>
        </form>
          </div>
        </div>
      </div>
    </div>
  </div>
</div>
</template>

<script>
import { mapActions } from 'vuex'
export default {
  data(){
    return {
      title:'',
      content:''
    }
  },
  props: ['item'],
  mounted() {
      this.$store.dispatch('getAllBlogs')
  },
  computed: {
    name () {
      return this.$store.state.userName
    },
    blogs () {
      return this.$store.state.blogs
    },
  },
  methods: {
    ...mapActions([
      'toggleBlog',
      'deleteBlog'
    ]),
    addBlog () {
      let title = this.title
      let content = this.content
      this.$store.dispatch('addBlog', { title, content })
      this.title = ''
      this.content = ''
    },
  }
}
</script>
