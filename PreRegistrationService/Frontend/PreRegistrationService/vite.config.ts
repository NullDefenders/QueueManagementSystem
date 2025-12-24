import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  server: {
    port: 5173,
    proxy: {
      '/api': {
        target: 'http://localhost:8087',
        changeOrigin: true,
        secure: false,
      },
      // Прокси для endpoints без /api префикса
      '/allrecordtimeinday': {
        target: 'http://localhost:8087',
        changeOrigin: true,
        secure: false,
      },
      '/record': {
        target: 'http://localhost:8087',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})
