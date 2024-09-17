export interface TransactionResponse {
    id: number,
    timestamp: Date,
    amount: number,
    description: string,
    account: string,
    category?: string
}

export interface TransactionsResponse {
    page: number,
    pageCount: number,
    totalPages: number,
    totalCount: number,
    transactions: TransactionResponse[]
}

export interface CategoriesResponse {
    categories: string[]
}
