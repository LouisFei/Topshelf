namespace Topshelf.Caching
{
    /// <summary>
    /// ������Ļص�����ǩ��
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="key"></param>
    /// <param name="value"></param>
    delegate void CacheItemCallback<in TKey, in TValue>(TKey key, TValue value);
}