<template>
  <AppLayout v-if="route.meta.requiresAuth">
    <RouterView />
  </AppLayout>
  <RouterView v-else />
</template>

<script setup lang="ts">
import { RouterView, useRoute } from 'vue-router'
import AppLayout from '@/components/layout/AppLayout.vue'

const route = useRoute()
</script>
