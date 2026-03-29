<template>
  <div class="completion-chart">
    <Bar :data="chartData" :options="chartOptions" />
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { Bar } from 'vue-chartjs'
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
} from 'chart.js'
import type { HabitTrendResponse } from '@/services/types'

ChartJS.register(CategoryScale, LinearScale, BarElement, Title, Tooltip)

const props = defineProps<{
  trend: HabitTrendResponse
}>()

const chartData = computed(() => ({
  labels: props.trend.dataPoints.map((p) => p.date),
  datasets: [
    {
      label: 'Completed',
      data: props.trend.dataPoints.map((p) => (p.completed ? 1 : 0)),
      backgroundColor: props.trend.dataPoints.map((p) =>
        p.completed ? 'rgba(34, 197, 94, 0.7)' : 'rgba(229, 231, 235, 0.5)'
      ),
      borderRadius: 4,
    },
  ],
}))

const chartOptions = {
  responsive: true,
  plugins: {
    title: {
      display: true,
      text: props.trend.habitName,
    },
    tooltip: {
      callbacks: {
        label: (context: { raw: number }) =>
          context.raw === 1 ? 'Completed' : 'Missed',
      },
    },
  },
  scales: {
    y: {
      display: false,
      max: 1,
    },
    x: {
      ticks: {
        maxRotation: 45,
        maxTicksLimit: 15,
      },
    },
  },
}
</script>

<style scoped>
.completion-chart {
  padding: 1rem;
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius);
}
</style>
