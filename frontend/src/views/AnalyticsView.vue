<template>
  <div class="analytics">
    <h1>Analytics</h1>

    <div v-if="isLoading" class="loading">Loading analytics…</div>

    <div v-else-if="summary" class="analytics-content">
      <div class="summary-cards">
        <div class="summary-card">
          <span class="summary-value">{{ summary.totalHabits }}</span>
          <span class="summary-label">Total Habits</span>
        </div>
        <div class="summary-card">
          <span class="summary-value">{{ summary.totalCompletionsThisWeek }}</span>
          <span class="summary-label">Completions This Week</span>
        </div>
        <div class="summary-card">
          <span class="summary-value">{{ summary.overallCompletionRate }}%</span>
          <span class="summary-label">Completion Rate</span>
        </div>
      </div>

      <h2 class="section-title">Streaks</h2>
      <div class="streak-list">
        <div
          v-for="streak in summary.habitStreaks"
          :key="streak.habitId"
          class="streak-card"
          :class="{ 'streak-card--selected': selectedHabitId === streak.habitId }"
          @click="selectHabit(streak.habitId)"
        >
          <h3 class="streak-habit-name">{{ streak.habitName }}</h3>
          <div class="streak-stats">
            <span class="streak-current">🔥 {{ streak.currentStreak }} day streak</span>
            <span class="streak-longest">Best: {{ streak.longestStreak }} days</span>
          </div>
        </div>
      </div>

      <div v-if="selectedTrend" class="trend-section">
        <h2 class="section-title">Completion Trend</h2>
        <CompletionChart :trend="selectedTrend" />
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { fetchAnalyticsSummary, fetchHabitTrend } from '@/services/analyticsService'
import type { AnalyticsSummaryResponse, HabitTrendResponse } from '@/services/types'
import CompletionChart from '@/components/CompletionChart.vue'

const summary = ref<AnalyticsSummaryResponse | null>(null)
const selectedHabitId = ref<string | null>(null)
const selectedTrend = ref<HabitTrendResponse | null>(null)
const isLoading = ref(false)

onMounted(async () => {
  isLoading.value = true
  try {
    summary.value = await fetchAnalyticsSummary()
  } finally {
    isLoading.value = false
  }
})

async function selectHabit(habitId: string) {
  selectedHabitId.value = habitId
  selectedTrend.value = await fetchHabitTrend(habitId, 30)
}
</script>

<style scoped>
.analytics {
  max-width: 800px;
  margin: 0 auto;
}

.loading {
  text-align: center;
  padding: 2rem;
  color: var(--color-text-muted);
}

.summary-cards {
  display: grid;
  grid-template-columns: repeat(3, 1fr);
  gap: 1rem;
  margin-bottom: 2rem;
}

.summary-card {
  display: flex;
  flex-direction: column;
  align-items: center;
  padding: 1.25rem;
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius);
}

.summary-value {
  font-size: 1.75rem;
  font-weight: 700;
}

.summary-label {
  font-size: 0.8rem;
  color: var(--color-text-muted);
  margin-top: 0.25rem;
}

.section-title {
  font-size: 1rem;
  font-weight: 600;
  color: var(--color-text-muted);
  margin-bottom: 0.75rem;
}

.streak-list {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
  margin-bottom: 2rem;
}

.streak-card {
  padding: 1rem 1.25rem;
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius);
  cursor: pointer;
  transition: border-color 0.15s;
}

.streak-card:hover {
  border-color: var(--color-primary);
}

.streak-card--selected {
  border-color: var(--color-primary);
  box-shadow: 0 0 0 1px var(--color-primary);
}

.streak-habit-name {
  font-size: 1rem;
  font-weight: 600;
  margin-bottom: 0.25rem;
}

.streak-stats {
  display: flex;
  gap: 1rem;
  font-size: 0.875rem;
  color: var(--color-text-muted);
}

.trend-section {
  margin-top: 1.5rem;
}
</style>