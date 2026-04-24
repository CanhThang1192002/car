import { z } from 'zod'

/** Chuẩn hóa origin API: bỏ / và /api cuối (tránh http://host:port/api -> axios/URL lệch đôi /api). */
export function normalizeApiBase(url: string): string {
  let u = url.trim().replace(/\/+$/, '')
  if (u.endsWith('/api')) u = u.slice(0, -4).replace(/\/+$/, '')
  return u || 'http://localhost:5239'
}

const envSchema = z.object({
  // Rỗng hoặc chỉ khoảng trắng → dùng default (tránh axios baseURL "" → gọi nhầm Vite → 404)
  VITE_API_BASE_URL: z.preprocess(
    (v) => {
      if (v === undefined || v === null) return undefined
      const s = String(v).trim()
      return s === '' ? undefined : s
    },
    z.string().optional().default('http://localhost:5239'),
  ),
})

const parsed = envSchema.parse(import.meta.env)

export const env = {
  VITE_API_BASE_URL: normalizeApiBase(parsed.VITE_API_BASE_URL ?? 'http://localhost:5239'),
}
