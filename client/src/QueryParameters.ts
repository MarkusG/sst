export class QueryParameters {
    pageSize?: number;
    page?: number = 1;
    sortField?: string;
    sortDirection?: "up" | "down";
    
    constructor(init?: Partial<QueryParameters>) {
        Object.assign(this, init);
    }

    toString(): string {
        const builder = [];

        if (this.pageSize) {
            builder.push(`?pageSize=${this.pageSize}`);
        }

        if (this.page) {
            const paramCharacter = builder.length === 0 ? '?' : '&';
            builder.push(`${paramCharacter}page=${this.page}`);
        }

        if (this.sortField) {
            const paramCharacter = builder.length === 0 ? '?' : '&';
            builder.push(`${paramCharacter}sortField=${this.sortField}`);
        }

        if (this.sortDirection) {
            const paramCharacter = builder.length === 0 ? '?' : '&';
            builder.push(`${paramCharacter}sortDirection=${this.sortDirection}`);
        }

        return builder.join('');
    }
}
